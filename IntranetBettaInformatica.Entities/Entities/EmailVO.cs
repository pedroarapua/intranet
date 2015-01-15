using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Net;
using System.Configuration;

namespace IntranetBettaInformatica.Entities.Entities
{
    /// <summary>
    /// Entidade responsável por informações de envio de email da aplicação
    /// </summary>
    public class EmailVO
    {
        #region construtores

        public EmailVO()
        {
            this.Usuarios = new List<UsuarioVO>();
            this.MailMessage = new MailMessage();
        }

        public EmailVO(ConfiguracoesSistemaVO conf) : this()
        {
            this.Configuracao = conf;
            if (!conf.Login.IsNullOrEmpty())
                this.MailMessage.From = new MailAddress(conf.Login, String.Format("Intranet - {0}", ConfigurationManager.AppSettings["EmpresaSistema"]));
            if (!conf.ServidoSmtp.IsNullOrEmpty())
            {
                this.Smtp = new SmtpClient(conf.ServidoSmtp);
                this.Smtp.EnableSsl = true;
                this.Smtp.Credentials = new NetworkCredential(conf.Login, conf.Senha);
            }
        }

        public EmailVO(ConfiguracoesSistemaVO conf, Boolean eReuniao, ReuniaoVO reuniao) : this(conf)
        {
            this.EReuniao = eReuniao;
            if (this.EReuniao)
            {
                this.Reuniao = reuniao;
                this.Subject = String.Format("Reunião - {0}", this.Reuniao.Codigo);
            }
        }

        #endregion

        #region propriedades

        public ConfiguracoesSistemaVO Configuracao { get; set; }

        public ReuniaoVO Reuniao { get; set; }

        public List<UsuarioVO> Usuarios { get; set; }

        public String EmailsTo { get { return String.Join(";", Usuarios.Where(x => !x.Email.IsNullOrEmpty()).ToList().Select(x => x.Email)); } }

        public Boolean EReuniao { get; set; }

        public Attachment Attachment { get; set; }

        public MailMessage MailMessage { get; set; }

        public SmtpClient Smtp { get; set; }

        public String Body { get; set; }

        public String Subject { get; set; }

        public String Particantes { 
            get 
            {
                String participantes = String.Empty;
                foreach (UsuarioVO u in this.Usuarios)
                {
                    participantes += String.Format("{0} ({1}), ", u.Nome, u.Email);
                }

                participantes = participantes.Replace(" ()", String.Empty);
                if (!participantes.IsNullOrEmpty())
                    participantes = participantes.Substring(0, participantes.Length - 2);
                return participantes;
            } 
        }

        #endregion

        #region metodos

        public void AddUsuariosAttachmentCollection()
        {
            foreach(UsuarioVO u in Usuarios)
            {
                if(!u.Email.IsNullOrEmpty())
                {
                    MailAddress mail = new MailAddress(u.Email, u.Nome);
                    this.MailMessage.To.Add(mail);
                }
            }
        }

        public void AddAttachment()
        {

            //  Set up the different mime types contained in the message
            System.Net.Mime.ContentType textType = new System.Net.Mime.ContentType("text/plain");
            System.Net.Mime.ContentType HTMLType = new System.Net.Mime.ContentType("text/html");
            System.Net.Mime.ContentType calendarType = new System.Net.Mime.ContentType("text/calendar");

            //  Add parameters to the calendar header
            calendarType.Parameters.Add("method", "REQUEST");
            calendarType.Parameters.Add("name", "meeting.ics");

            //  Create message body parts
            //  create the Body in text format
            string bodyText = "<b>Reunião:</b> {0}\r\n<b>Título:</b> {1}\r\n<b>Descrição:</b> {2}\r\n<b>Início:</b> {3}\r\n<b>Término:</b> {4}\r\n<b>Local:</b> {5}\r\n<b>Participantes:</b> {6}";
            bodyText = string.Format(bodyText,
                this.Reuniao.Codigo,
                this.Reuniao.Titulo,
                this.Reuniao.Descricao,
                this.Reuniao.DataInicial.ToLongDateString() + " às " + this.Reuniao.DataInicial.ToLongTimeString(),
                this.Reuniao.DataFinal.ToLongDateString() + " às " + this.Reuniao.DataFinal.ToLongTimeString(),
                this.Reuniao.SalaReuniao.Nome,
                this.Reuniao.Participantes);

            AlternateView textView = AlternateView.CreateAlternateViewFromString(bodyText, textType);
            this.MailMessage.AlternateViews.Add(textView);

            //create the Body in HTML format
            string bodyHTML = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 3.2//EN\">\r\n<HTML>\r\n<HEAD>\r\n<META HTTP-EQUIV=\"Content-Type\" CONTENT=\"text/html; charset=utf-8\">\r\n<META NAME=\"Generator\" CONTENT=\"MS Exchange Server version 6.5.7652.24\">\r\n<TITLE>{0}</TITLE>\r\n</HEAD>\r\n<BODY>\r\n<!-- Converted from text/plain format -->\r\n<P><FONT SIZE=2><b>Reunião:</b> {0}<BR>\r\n<b>Título:</b> {1}<BR>\r\n<b>Descrição:</b> {2}<BR>\r\n<b>Início:</b> {3}<BR>\r\n<b>Término:</b> {4}<BR>\r\n<b>Local: </b>{5}<BR>\r\n<b>Participantes:</b> {6}<BR></FONT>\r\n</P></BODY>\r\n</HTML>";
            bodyHTML = string.Format(bodyHTML,
                this.Reuniao.Codigo,
                this.Reuniao.Titulo,
                this.Reuniao.Descricao,
                this.Reuniao.DataInicial.ToLongDateString() + " às " + this.Reuniao.DataInicial.ToLongTimeString(),
                this.Reuniao.DataFinal.ToLongDateString() + " às " + this.Reuniao.DataFinal.ToLongTimeString(),
                this.Reuniao.SalaReuniao.Nome,
                this.Particantes);

            AlternateView HTMLView = AlternateView.CreateAlternateViewFromString(bodyHTML, HTMLType);
            this.MailMessage.AlternateViews.Add(HTMLView);

            //create the Body in VCALENDAR format
            string calDateFormat = "yyyyMMddTHHmmssZ";
            string bodyCalendar = "BEGIN:VCALENDAR\r\nMETHOD:REQUEST\r\nPRODID:Microsoft CDO for Microsoft Exchange\r\nVERSION:2.0\r\nBEGIN:VTIMEZONE\r\nTZID:(GMT-06.00) Central Time (US & Canada)\r\nX-MICROSOFT-CDO-TZID:11\r\nBEGIN:STANDARD\r\nDTSTART:16010101T020000\r\nTZOFFSETFROM:-0500\r\nTZOFFSETTO:-0600\r\nRRULE:FREQ=YEARLY;WKST=MO;INTERVAL=1;BYMONTH=11;BYDAY=1SU\r\nEND:STANDARD\r\nBEGIN:DAYLIGHT\r\nDTSTART:16010101T020000\r\nTZOFFSETFROM:-0600\r\nTZOFFSETTO:-0500\r\nRRULE:FREQ=YEARLY;WKST=MO;INTERVAL=1;BYMONTH=3;BYDAY=2SU\r\nEND:DAYLIGHT\r\nEND:VTIMEZONE\r\nBEGIN:VEVENT\r\nDTSTAMP:{8}\r\nDTSTART:{0}\r\nSUMMARY:{7}\r\nUID:{5}\r\nATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION;RSVP=TRUE;CN=\"{9}\":MAILTO:{9}\r\nACTION;RSVP=TRUE;CN=\"{4}\":MAILTO:{4}\r\nORGANIZER;CN=\"{3}\":mailto:{4}\r\nLOCATION:{2}\r\nDTEND:{1}\r\nDESCRIPTION:{7}\\N\r\nSEQUENCE:1\r\nPRIORITY:5\r\nCLASS:\r\nCREATED:{8}\r\nLAST-MODIFIED:{8}\r\nSTATUS:CONFIRMED\r\nTRANSP:OPAQUE\r\nX-MICROSOFT-CDO-BUSYSTATUS:BUSY\r\nX-MICROSOFT-CDO-INSTTYPE:0\r\nX-MICROSOFT-CDO-INTENDEDSTATUS:BUSY\r\nX-MICROSOFT-CDO-ALLDAYEVENT:FALSE\r\nX-MICROSOFT-CDO-IMPORTANCE:1\r\nX-MICROSOFT-CDO-OWNERAPPTID:-1\r\nX-MICROSOFT-CDO-ATTENDEE-CRITICAL-CHANGE:{8}\r\nX-MICROSOFT-CDO-OWNER-CRITICAL-CHANGE:{8}\r\nBEGIN:VALARM\r\nACTION:DISPLAY\r\nDESCRIPTION:REMINDER\r\nTRIGGER;RELATED=START:-PT00H15M00S\r\nEND:VALARM\r\nEND:VEVENT\r\nEND:VCALENDAR\r\n";
            bodyCalendar = string.Format(bodyCalendar,
                this.Reuniao.DataInicial.ToUniversalTime().ToString(calDateFormat),
                this.Reuniao.DataFinal.ToUniversalTime().ToString(calDateFormat),
                this.Reuniao.SalaReuniao.Nome,
				String.Format("{0} - {1}", this.Configuracao.Descricao, ConfigurationManager.AppSettings["EmpresaSistema"]),
                this.Configuracao.Login,
                this.Reuniao.UID,
                "Reunião - " + this.Reuniao.Codigo,
                "Reunião - " + this.Reuniao.Codigo,
                DateTime.Now.ToUniversalTime().ToString(calDateFormat),
                this.MailMessage.To.ToString());

            AlternateView calendarView = AlternateView.CreateAlternateViewFromString(bodyCalendar, calendarType);
            calendarView.TransferEncoding = TransferEncoding.SevenBit;
            this.MailMessage.AlternateViews.Add(calendarView);

        }

        public void RemoverUsuariosReuniao()
        {
            //  Set up the different mime types contained in the message
            System.Net.Mime.ContentType textType = new System.Net.Mime.ContentType("text/plain");
            System.Net.Mime.ContentType HTMLType = new System.Net.Mime.ContentType("text/html");
            System.Net.Mime.ContentType calendarType = new System.Net.Mime.ContentType("text/calendar");

            //  Add parameters to the calendar header
            calendarType.Parameters.Add("method", "CANCEL");
            calendarType.Parameters.Add("name", "meeting.ics");

            //  Create message body parts
            //  create the Body in text format
            string bodyText = "<b>Reunião:</b> {0} (Remoção de Usuário)\r\n<b>Título:</b> {1}\r\n<b>Descrição:</b> {2}\r\n<b>Início:</b> {3}\r\n<b>Término:</b> {4}\r\n<b>Local:</b> {5}\r\n<b>";
            bodyText = string.Format(bodyText,
                this.Reuniao.Codigo,
                this.Reuniao.Titulo,
                this.Reuniao.Descricao,
                this.Reuniao.DataInicial.ToLongDateString() + " às " + this.Reuniao.DataInicial.ToLongTimeString(),
                this.Reuniao.DataFinal.ToLongDateString() + " às " + this.Reuniao.DataFinal.ToLongTimeString(),
                this.Reuniao.SalaReuniao.Nome);

            AlternateView textView = AlternateView.CreateAlternateViewFromString(bodyText, textType);
            this.MailMessage.AlternateViews.Add(textView);

            //create the Bometdy in HTML format
            string bodyHTML = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 3.2//EN\">\r\n<HTML>\r\n<HEAD>\r\n<META HTTP-EQUIV=\"Content-Type\" CONTENT=\"text/html; charset=utf-8\">\r\n<META NAME=\"Generator\" CONTENT=\"MS Exchange Server version 6.5.7652.24\">\r\n<TITLE>{0}</TITLE>\r\n</HEAD>\r\n<BODY>\r\n<!-- Converted from text/plain format -->\r\n<P><FONT SIZE=2><b>Você não faz mais parte desta reunião.</b><BR><BR><b>Reunião:</b> {0} (Remoção de Usuário)<BR>\r\n<b>Título:</b> {1}<BR>\r\n<b>Descrição:</b> {2}<BR>\r\n<b>Início:</b> {3}<BR>\r\n<b>Término:</b> {4}<BR>\r\n<b>Local: </b>{5}<BR>\r\n<b></FONT>\r\n</P></BODY>\r\n</HTML>";
            bodyHTML = string.Format(bodyHTML,
                this.Reuniao.Codigo,
                this.Reuniao.Titulo,
                this.Reuniao.Descricao,
                this.Reuniao.DataInicial.ToLongDateString() + " às " + this.Reuniao.DataInicial.ToLongTimeString(),
                this.Reuniao.DataFinal.ToLongDateString() + " às " + this.Reuniao.DataFinal.ToLongTimeString(),
                this.Reuniao.SalaReuniao.Nome);

            AlternateView HTMLView = AlternateView.CreateAlternateViewFromString(bodyHTML, HTMLType);
            this.MailMessage.AlternateViews.Add(HTMLView);

            //create the Body in VCALENDAR format
            string calDateFormat = "yyyyMMddTHHmmssZ";
            string bodyCalendar = "BEGIN:VCALENDAR\r\nMETHOD:Cancel\r\nPRODID:Microsoft CDO for Microsoft Exchange\r\nVERSION:2.0\r\nBEGIN:VTIMEZONE\r\nTZID:(GMT-06.00) Central Time (US & Canada)\r\nX-MICROSOFT-CDO-TZID:11\r\nBEGIN:STANDARD\r\nDTSTART:16010101T020000\r\nTZOFFSETFROM:-0500\r\nTZOFFSETTO:-0600\r\nRRULE:FREQ=YEARLY;WKST=MO;INTERVAL=1;BYMONTH=11;BYDAY=1SU\r\nEND:STANDARD\r\nBEGIN:DAYLIGHT\r\nDTSTART:16010101T020000\r\nTZOFFSETFROM:-0600\r\nTZOFFSETTO:-0500\r\nRRULE:FREQ=YEARLY;WKST=MO;INTERVAL=1;BYMONTH=3;BYDAY=2SU\r\nEND:DAYLIGHT\r\nEND:VTIMEZONE\r\nBEGIN:VEVENT\r\nDTSTAMP:{8}\r\nDTSTART:{0}\r\nSUMMARY:{7}\r\nUID:{5}\r\nATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION;RSVP=TRUE;CN=\"{9}\":MAILTO:{9}\r\nACTION;RSVP=TRUE;CN=\"{4}\":MAILTO:{4}\r\nORGANIZER;CN=\"{3}\":mailto:{4}\r\nLOCATION:{2}\r\nDTEND:{1}\r\nDESCRIPTION:{7}\\N\r\nSEQUENCE:1\r\nPRIORITY:5\r\nCLASS:\r\nCREATED:{8}\r\nLAST-MODIFIED:{8}\r\nSTATUS:CONFIRMED\r\nTRANSP:OPAQUE\r\nX-MICROSOFT-CDO-BUSYSTATUS:BUSY\r\nX-MICROSOFT-CDO-INSTTYPE:0\r\nX-MICROSOFT-CDO-INTENDEDSTATUS:BUSY\r\nX-MICROSOFT-CDO-ALLDAYEVENT:FALSE\r\nX-MICROSOFT-CDO-IMPORTANCE:1\r\nX-MICROSOFT-CDO-OWNERAPPTID:-1\r\nX-MICROSOFT-CDO-ATTENDEE-CRITICAL-CHANGE:{8}\r\nX-MICROSOFT-CDO-OWNER-CRITICAL-CHANGE:{8}\r\nBEGIN:VALARM\r\nACTION:DISPLAY\r\nDESCRIPTION:REMINDER\r\nTRIGGER;RELATED=START:-PT00H15M00S\r\nEND:VALARM\r\nEND:VEVENT\r\nEND:VCALENDAR\r\n";
            bodyCalendar = string.Format(bodyCalendar,
                this.Reuniao.DataInicial.ToUniversalTime().ToString(calDateFormat),
                this.Reuniao.DataFinal.ToUniversalTime().ToString(calDateFormat),
                this.Reuniao.SalaReuniao.Nome,
				String.Format("{0} - {1}", this.Configuracao.Descricao, ConfigurationManager.AppSettings["EmpresaSistema"]),
                this.Configuracao.Login,
                this.Reuniao.UID,
                "Reunião - " + this.Reuniao.Codigo,
                "Reunião - " + this.Reuniao.Codigo,
                DateTime.Now.ToUniversalTime().ToString(calDateFormat),
                this.MailMessage.To.ToString());

            AlternateView calendarView = AlternateView.CreateAlternateViewFromString(bodyCalendar, calendarType);
            calendarView.TransferEncoding = TransferEncoding.SevenBit;
            this.Subject += " (Remoção de Usuário)";
            this.MailMessage.AlternateViews.Add(calendarView);
        }

        public void CancelamentoReuniao()
        {
            //  Set up the different mime types contained in the message
            System.Net.Mime.ContentType textType = new System.Net.Mime.ContentType("text/plain");
            System.Net.Mime.ContentType HTMLType = new System.Net.Mime.ContentType("text/html");
            System.Net.Mime.ContentType calendarType = new System.Net.Mime.ContentType("text/calendar");

            //  Add parameters to the calendar header
            calendarType.Parameters.Add("method", "CANCEL");
            calendarType.Parameters.Add("name", "meeting.ics");

            //  Create message body parts
            //  create the Body in text format
            string bodyText = "<b>Reunião:</b> {0} (Cancelada)\r\n<b>Título:</b> {1}\r\n<b>Descrição:</b> {2}\r\n<b>Início:</b> {3}\r\n<b>Término:</b> {4}\r\n<b>Local:</b> {5}\r\n<b>Participantes:</b> {6}";
            bodyText = string.Format(bodyText,
                this.Reuniao.Codigo,
                this.Reuniao.Titulo,
                this.Reuniao.Descricao,
                this.Reuniao.DataInicial.ToLongDateString() + " às " + this.Reuniao.DataInicial.ToLongTimeString(),
                this.Reuniao.DataFinal.ToLongDateString() + " às " + this.Reuniao.DataFinal.ToLongTimeString(),
                this.Reuniao.SalaReuniao.Nome,
                this.Reuniao.Participantes);

            AlternateView textView = AlternateView.CreateAlternateViewFromString(bodyText, textType);
            this.MailMessage.AlternateViews.Add(textView);

            //create the Bometdy in HTML format
            string bodyHTML = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 3.2//EN\">\r\n<HTML>\r\n<HEAD>\r\n<META HTTP-EQUIV=\"Content-Type\" CONTENT=\"text/html; charset=utf-8\">\r\n<META NAME=\"Generator\" CONTENT=\"MS Exchange Server version 6.5.7652.24\">\r\n<TITLE>{0}</TITLE>\r\n</HEAD>\r\n<BODY>\r\n<!-- Converted from text/plain format -->\r\n<P><FONT SIZE=2><b>Reunião:</b> {0} (Cancelada)<BR>\r\n<b>Título:</b> {1}<BR>\r\n<b>Descrição:</b> {2}<BR>\r\n<b>Início:</b> {3}<BR>\r\n<b>Término:</b> {4}<BR>\r\n<b>Local: </b>{5}<BR>\r\n<b>Participantes:</b> {6}<BR></FONT>\r\n</P></BODY>\r\n</HTML>";
            bodyHTML = string.Format(bodyHTML,
                this.Reuniao.Codigo,
                this.Reuniao.Titulo,
                this.Reuniao.Descricao,
                this.Reuniao.DataInicial.ToLongDateString() + " às " + this.Reuniao.DataInicial.ToLongTimeString(),
                this.Reuniao.DataFinal.ToLongDateString() + " às " + this.Reuniao.DataFinal.ToLongTimeString(),
                this.Reuniao.SalaReuniao.Nome,
                this.Particantes);

            AlternateView HTMLView = AlternateView.CreateAlternateViewFromString(bodyHTML, HTMLType);
            this.MailMessage.AlternateViews.Add(HTMLView);

            //create the Body in VCALENDAR format
            string calDateFormat = "yyyyMMddTHHmmssZ";
            string bodyCalendar = "BEGIN:VCALENDAR\r\nMETHOD:Cancel\r\nPRODID:Microsoft CDO for Microsoft Exchange\r\nVERSION:2.0\r\nBEGIN:VTIMEZONE\r\nTZID:(GMT-06.00) Central Time (US & Canada)\r\nX-MICROSOFT-CDO-TZID:11\r\nBEGIN:STANDARD\r\nDTSTART:16010101T020000\r\nTZOFFSETFROM:-0500\r\nTZOFFSETTO:-0600\r\nRRULE:FREQ=YEARLY;WKST=MO;INTERVAL=1;BYMONTH=11;BYDAY=1SU\r\nEND:STANDARD\r\nBEGIN:DAYLIGHT\r\nDTSTART:16010101T020000\r\nTZOFFSETFROM:-0600\r\nTZOFFSETTO:-0500\r\nRRULE:FREQ=YEARLY;WKST=MO;INTERVAL=1;BYMONTH=3;BYDAY=2SU\r\nEND:DAYLIGHT\r\nEND:VTIMEZONE\r\nBEGIN:VEVENT\r\nDTSTAMP:{8}\r\nDTSTART:{0}\r\nSUMMARY:{7}\r\nUID:{5}\r\nATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION;RSVP=TRUE;CN=\"{9}\":MAILTO:{9}\r\nACTION;RSVP=TRUE;CN=\"{4}\":MAILTO:{4}\r\nORGANIZER;CN=\"{3}\":mailto:{4}\r\nLOCATION:{2}\r\nDTEND:{1}\r\nDESCRIPTION:{7}\\N\r\nSEQUENCE:1\r\nPRIORITY:5\r\nCLASS:\r\nCREATED:{8}\r\nLAST-MODIFIED:{8}\r\nSTATUS:CONFIRMED\r\nTRANSP:OPAQUE\r\nX-MICROSOFT-CDO-BUSYSTATUS:BUSY\r\nX-MICROSOFT-CDO-INSTTYPE:0\r\nX-MICROSOFT-CDO-INTENDEDSTATUS:BUSY\r\nX-MICROSOFT-CDO-ALLDAYEVENT:FALSE\r\nX-MICROSOFT-CDO-IMPORTANCE:1\r\nX-MICROSOFT-CDO-OWNERAPPTID:-1\r\nX-MICROSOFT-CDO-ATTENDEE-CRITICAL-CHANGE:{8}\r\nX-MICROSOFT-CDO-OWNER-CRITICAL-CHANGE:{8}\r\nBEGIN:VALARM\r\nACTION:DISPLAY\r\nDESCRIPTION:REMINDER\r\nTRIGGER;RELATED=START:-PT00H15M00S\r\nEND:VALARM\r\nEND:VEVENT\r\nEND:VCALENDAR\r\n";
            bodyCalendar = string.Format(bodyCalendar,
                this.Reuniao.DataInicial.ToUniversalTime().ToString(calDateFormat),
                this.Reuniao.DataFinal.ToUniversalTime().ToString(calDateFormat),
                this.Reuniao.SalaReuniao.Nome,
				String.Format("{0} - {1}", this.Configuracao.Descricao, ConfigurationManager.AppSettings["EmpresaSistema"]),
                this.Configuracao.Login,
                this.Reuniao.UID,
                "Reunião - " + this.Reuniao.Codigo,
                "Reunião - " + this.Reuniao.Codigo,
                DateTime.Now.ToUniversalTime().ToString(calDateFormat),
                this.MailMessage.To.ToString());

            AlternateView calendarView = AlternateView.CreateAlternateViewFromString(bodyCalendar, calendarType);
            calendarView.TransferEncoding = TransferEncoding.SevenBit;
            this.Subject += " (Cancelada)";
            this.MailMessage.AlternateViews.Add(calendarView);
        }

        public void RemovidoReuniao()
        {

            //  Set up the different mime types contained in the message
            System.Net.Mime.ContentType textType = new System.Net.Mime.ContentType("text/plain");
            System.Net.Mime.ContentType HTMLType = new System.Net.Mime.ContentType("text/html");
            System.Net.Mime.ContentType calendarType = new System.Net.Mime.ContentType("text/calendar");

            //  Add parameters to the calendar header
            calendarType.Parameters.Add("method", "CANCEL");
            calendarType.Parameters.Add("name", "meeting.ics");

            //  Create message body parts
            //  create the Body in text format
            string bodyText = "<b>Reunião:</b> {0} (Removida)\r\n<b>Título:</b> {1}\r\n<b>Descrição:</b> {2}\r\n<b>Início:</b> {3}\r\n<b>Término:</b> {4}\r\n<b>Local:</b> {5}\r\n<b>Participantes:</b> {6}";
            bodyText = string.Format(bodyText,
                this.Reuniao.Codigo,
                this.Reuniao.Titulo,
                this.Reuniao.Descricao,
                this.Reuniao.DataInicial.ToLongDateString() + " às " + this.Reuniao.DataInicial.ToLongTimeString(),
                this.Reuniao.DataFinal.ToLongDateString() + " às " + this.Reuniao.DataFinal.ToLongTimeString(),
                this.Reuniao.SalaReuniao.Nome,
                this.Reuniao.Participantes);

            AlternateView textView = AlternateView.CreateAlternateViewFromString(bodyText, textType);
            this.MailMessage.AlternateViews.Add(textView);

            //create the Bometdy in HTML format
            string bodyHTML = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 3.2//EN\">\r\n<HTML>\r\n<HEAD>\r\n<META HTTP-EQUIV=\"Content-Type\" CONTENT=\"text/html; charset=utf-8\">\r\n<META NAME=\"Generator\" CONTENT=\"MS Exchange Server version 6.5.7652.24\">\r\n<TITLE>{0}</TITLE>\r\n</HEAD>\r\n<BODY>\r\n<!-- Converted from text/plain format -->\r\n<P><FONT SIZE=2><b>Reunião:</b> {0} (Removida)<BR>\r\n<b>Título:</b> {1}<BR>\r\n<b>Descrição:</b> {2}<BR>\r\n<b>Início:</b> {3}<BR>\r\n<b>Término:</b> {4}<BR>\r\n<b>Local: </b>{5}<BR>\r\n<b>Participantes:</b> {6}<BR></FONT>\r\n</P></BODY>\r\n</HTML>";
            bodyHTML = string.Format(bodyHTML,
                this.Reuniao.Codigo,
                this.Reuniao.Titulo,
                this.Reuniao.Descricao,
                this.Reuniao.DataInicial.ToLongDateString() + " às " + this.Reuniao.DataInicial.ToLongTimeString(),
                this.Reuniao.DataFinal.ToLongDateString() + " às " + this.Reuniao.DataFinal.ToLongTimeString(),
                this.Reuniao.SalaReuniao.Nome,
                this.Particantes);

            AlternateView HTMLView = AlternateView.CreateAlternateViewFromString(bodyHTML, HTMLType);
            this.MailMessage.AlternateViews.Add(HTMLView);

            //create the Body in VCALENDAR format
            string calDateFormat = "yyyyMMddTHHmmssZ";
            string bodyCalendar = "BEGIN:VCALENDAR\r\nMETHOD:Cancel\r\nPRODID:Microsoft CDO for Microsoft Exchange\r\nVERSION:2.0\r\nBEGIN:VTIMEZONE\r\nTZID:(GMT-06.00) Central Time (US & Canada)\r\nX-MICROSOFT-CDO-TZID:11\r\nBEGIN:STANDARD\r\nDTSTART:16010101T020000\r\nTZOFFSETFROM:-0500\r\nTZOFFSETTO:-0600\r\nRRULE:FREQ=YEARLY;WKST=MO;INTERVAL=1;BYMONTH=11;BYDAY=1SU\r\nEND:STANDARD\r\nBEGIN:DAYLIGHT\r\nDTSTART:16010101T020000\r\nTZOFFSETFROM:-0600\r\nTZOFFSETTO:-0500\r\nRRULE:FREQ=YEARLY;WKST=MO;INTERVAL=1;BYMONTH=3;BYDAY=2SU\r\nEND:DAYLIGHT\r\nEND:VTIMEZONE\r\nBEGIN:VEVENT\r\nDTSTAMP:{8}\r\nDTSTART:{0}\r\nSUMMARY:{7}\r\nUID:{5}\r\nATTENDEE;ROLE=REQ-PARTICIPANT;PARTSTAT=NEEDS-ACTION;RSVP=TRUE;CN=\"{9}\":MAILTO:{9}\r\nACTION;RSVP=TRUE;CN=\"{4}\":MAILTO:{4}\r\nORGANIZER;CN=\"{3}\":mailto:{4}\r\nLOCATION:{2}\r\nDTEND:{1}\r\nDESCRIPTION:{7}\\N\r\nSEQUENCE:1\r\nPRIORITY:5\r\nCLASS:\r\nCREATED:{8}\r\nLAST-MODIFIED:{8}\r\nSTATUS:CONFIRMED\r\nTRANSP:OPAQUE\r\nX-MICROSOFT-CDO-BUSYSTATUS:BUSY\r\nX-MICROSOFT-CDO-INSTTYPE:0\r\nX-MICROSOFT-CDO-INTENDEDSTATUS:BUSY\r\nX-MICROSOFT-CDO-ALLDAYEVENT:FALSE\r\nX-MICROSOFT-CDO-IMPORTANCE:1\r\nX-MICROSOFT-CDO-OWNERAPPTID:-1\r\nX-MICROSOFT-CDO-ATTENDEE-CRITICAL-CHANGE:{8}\r\nX-MICROSOFT-CDO-OWNER-CRITICAL-CHANGE:{8}\r\nBEGIN:VALARM\r\nACTION:DISPLAY\r\nDESCRIPTION:REMINDER\r\nTRIGGER;RELATED=START:-PT00H15M00S\r\nEND:VALARM\r\nEND:VEVENT\r\nEND:VCALENDAR\r\n";
            bodyCalendar = string.Format(bodyCalendar,
                this.Reuniao.DataInicial.ToUniversalTime().ToString(calDateFormat),
                this.Reuniao.DataFinal.ToUniversalTime().ToString(calDateFormat),
                this.Reuniao.SalaReuniao.Nome,
                String.Format("{0} - {1}", this.Configuracao.Descricao, ConfigurationManager.AppSettings["EmpresaSistema"]),
                this.Configuracao.Login,
                this.Reuniao.UID,
                "Reunião - " + this.Reuniao.Codigo,
                "Reunião - " + this.Reuniao.Codigo,
                DateTime.Now.ToUniversalTime().ToString(calDateFormat),
                this.MailMessage.To.ToString());

            AlternateView calendarView = AlternateView.CreateAlternateViewFromString(bodyCalendar, calendarType);
            calendarView.TransferEncoding = TransferEncoding.SevenBit;
            this.Subject += " (Removida)";
            this.MailMessage.AlternateViews.Add(calendarView);
        }

		#endregion
    }
}
