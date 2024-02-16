using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class JoinBehavior : MonoBehaviour
{
    public string email;
    public string password;
    public RawImage myImage;
    private bool bWaitingForAuth = false;

    private FirebaseAuth auth; // ���� ��ü �ҷ�����

    [SerializeField] private Button signInButton;
    [SerializeField] private Button signUpButton;
    [SerializeField] private Button googleLoginButton;
    [SerializeField] private Button googleLogoutButton;
    [SerializeField] private Text resultText;
    [SerializeField] private Text googleText;

    private void Awake()
    {
        // ���� ���Ӽ��� Ȱ��ȭ (�ʱ�ȭ)
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance; // ���� ��ü �ʱ�ȭ

        /*signInButton.onClick.AddListener(delegate
        {
            SignIn(email, password);
        });

        signUpButton.onClick.AddListener(delegate
        {
            SignUp(email, password);
        });*/

        googleLoginButton.onClick.AddListener(OnBtnLoginClicked);
        //googleLogoutButton.onClick.AddListener(OnBtnLogoutClicked);
    }

    void SignUp(string email, string password)
    {
        // �̸��ϰ� ��й�ȣ�� �����ϴ� �Լ�
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(
             task => {
                 if (!task.IsCanceled && !task.IsFaulted)
                 {
                     resultText.text = email + " �� ȸ������ �ϼ̽��ϴ�.";
                 }
                 else
                 {
                     resultText.text = "ȸ�����Կ� �����ϼ̽��ϴ�.";
                 }
             }
         );
    }

    public void AutoGooglePlayGamesLogin()
    {
        googleText.text = "...";
        if (bWaitingForAuth)
            return;
        //���� �α����� �Ǿ����� �ʴٸ� 
        if (!Social.localUser.authenticated)
        {
            googleText.text = "Authenticating...";
            bWaitingForAuth = true;
            //�α��� ���� ó������ (�ݹ��Լ�)
            Social.localUser.Authenticate(AuthenticateCallback);
        }
        else
        {
            googleText.text = "Login Fail\n";
        }
    }

    void SignIn(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                resultText.text = "�α��ο� �����Ͽ����ϴ�.";
                return;
            }
            if (task.IsFaulted)
            {
                resultText.text = "SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception;
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            resultText.text = "�α��� ����" + result.User.DisplayName + ", " + result.User.UserId;
        });
    }

    // �����α��� 
    public void OnBtnLoginClicked()
    {
        //�̹� ������ ����ڴ� �ٷ� �α��� �����ȴ�. 
        if (Social.localUser.authenticated)
        {
            Debug.Log(Social.localUser.userName);
            googleText.text = "name : " + Social.localUser.userName + "\n";
        }
        else
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log(Social.localUser.userName);
                    googleText.text = "name : " + Social.localUser.userName + "\n";
                    StartCoroutine(TryFirebaseLogin());
                }
                else
                {
                    Debug.Log("Login Fail");
                    googleText.text = "Login Fail\n";
                }
            });
    }


    // ���� �α׾ƿ� 
    public void OnBtnLogoutClicked()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        googleText.text = "LogOut...";
    }


    // ���� callback
    void AuthenticateCallback(bool success)
    {
        googleText.text = "Loading";
        if (success)
        {
            // ����� �̸��� ����� 
            googleText.text = "Welcome" + Social.localUser.userName + "\n";
            StartCoroutine(UserPictureLoad());
        }
        else
        {
            googleText.text = "Login Fail\n";
        }
    }

    // ���� �̹��� �޾ƿ��� 
    IEnumerator UserPictureLoad()
    {
        googleText.text = "image Loading ...";
        // ���� ���� �̹��� ��������
        Texture2D pic = Social.localUser.image;

        // ���� �ƹ�Ÿ �̹��� ���θ� Ȯ�� �� �̹��� ���� 
        while (pic == null)
        {
            pic = Social.localUser.image;
            yield return null;
        }
        //myImage.texture = pic;
        googleText.text = "image Create";
    }

    IEnumerator TryFirebaseLogin()
    {
        Debug.Log("(PlayGamesLocalUser)Social.localUser).GetIdToken()" + ((PlayGamesLocalUser)Social.localUser).GetIdToken());
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
            yield return null;
        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();

        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Debug.Log("Success!");
        });
    }
}
