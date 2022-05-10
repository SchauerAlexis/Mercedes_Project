using System.Collections;
using UnityEngine.Networking;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Email : MonoBehaviour
{
    [Header("Email Data")]
    public TextMeshProUGUI mailTitle;
    public TextMeshProUGUI mailText;

    [Header("World Interaction Fields")]
    [SerializeField] Button btnSubmit;
    [SerializeField] bool sendDirect;

    [Header("Email Credentials")]
    [SerializeField] private string usedServer = "smtp.gmail.com";
    [SerializeField] private int port = 465;
    [SerializeField] private string SenderEmailAddress = "example@gmail.com";
    [SerializeField] private string SenderPassword = "example!";
    public string ReceiverEmailAddress = "example@gmail.com";

    // Method 2: Server request methode
    const string url = "Server PHP file";

    void Start()
    {
        UnityEngine.Assertions.Assert.IsNotNull(mailText);
        UnityEngine.Assertions.Assert.IsNotNull(btnSubmit);
    }

    // Method 1: Direct message
    public void SendAnEmail()
    {
        StartCoroutine(SendMailRoutine());
    }

    private IEnumerator SendMailRoutine()
    {
        string message = mailText.text;
        string title = mailTitle.text;

        // Create mail
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(SenderEmailAddress);
        mail.To.Add(ReceiverEmailAddress);
        mail.Subject = title;
        mail.Body = message;
        yield return new WaitForSeconds(1f);

        // Setup server 
        SmtpClient smtpServer = new SmtpClient(usedServer);
        smtpServer.Port = port;
        smtpServer.Credentials = new NetworkCredential(
            SenderEmailAddress, SenderPassword) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        yield return new WaitForSeconds(1f);

        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                Debug.Log("Email success!");
                return true;
            };
        yield return new WaitForSeconds(1f);

        // Send mail to server, print results
        try
        {
            smtpServer.Send(mail);
        }
        catch (System.Exception e)
        {
            Debug.Log("Email error: " + e.Message);
        }
        finally
        {
            Debug.Log("Email finish!");
        }
    }

    /*
     * *
     * *
    // Method 2: Server request
    private void SendServerRequestForEmail(string message, string title)
    {
        StartCoroutine(SendMailRequestToServer(message, title));
    }

    // Method 2: Server request
    private IEnumerator SendMailRequestToServer(string message, string title)
    {
        // Setup form responses
        WWWForm form = new WWWForm();
        form.AddField("name", "It's me!");
        form.AddField("fromEmail", SenderEmailAddress);
        form.AddField("toEmail", ReceiverEmailAddress);
        form.AddField("title", title);
        form.AddField("message", message);

        // Submit form to our server, then wait
        UnityWebRequest uwr = UnityWebRequest.Post(url, form);
        Debug.Log("Email sent!");

        yield return uwr;

        // Print results
        if (uwr.error == null)
        {
            Debug.Log("WWW Success!: " + uwr.downloadHandler.text);
        }
        else
        {
            Debug.Log("WWW Error: " + uwr.error);
        }
    }
    * *
    * *
    */
}
