using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetBettaInformatica.Entities.Entities;
using System.Net.Mail;
using System.Threading;

namespace IntranetBettaInformatica.Business
{
    /// <summary>
    /// Entidade para envio de emails
    /// </summary>
    public class EmailBO
    {
        #region metodoso

        public void EnviarEmailAssincrono(EmailVO email)
        {
            new Thread(
                new ThreadStart(
                    delegate() {
                        try
                        {
                            if (!String.IsNullOrEmpty(email.Configuracao.ServidoSmtp) && email.MailMessage.To.Count > 0)
                            {
                                email.Smtp.Send(email.MailMessage);
                            }
                        }
                        catch (Exception ex)
                        {
                            
                        }
                    }
                )
            ).Start();
        }

		public void EnviarEmailSincrono(EmailVO email)
		{
			try
			{
				email.MailMessage.Body = email.Body;
				email.MailMessage.Subject = email.Subject;
				if (!String.IsNullOrEmpty(email.Configuracao.ServidoSmtp) && email.MailMessage.To.Count > 0)
				{
					email.Smtp.Send(email.MailMessage);
				}
				else
				{
					if (String.IsNullOrEmpty(email.Configuracao.ServidoSmtp))
						throw new Exception("Smtp não configurado, informe o moderador do sistema.");
				}
			}
			catch (SmtpException ex) {
				throw new Exception("Configurações de SMTP inválidas, informe o moderador do sistema.");
			}
			catch (Exception ex1)
			{
				throw ex1;
			}
		}

        #endregion
    }
}
