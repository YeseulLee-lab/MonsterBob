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

    private FirebaseAuth auth; // 인증 객체 불러오기

    [SerializeField] private Button signInButton;
    [SerializeField] private Button signUpButton;
    [SerializeField] private Button googleLoginButton;
    [SerializeField] private Button googleLogoutButton;
    [SerializeField] private Text resultText;
    [SerializeField] private Text googleText;

    private void Awake()
    {
        // 구글 게임서비스 활성화 (초기화)
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance; // 인증 객체 초기화

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
        // 이메일과 비밀번호로 가입하는 함수
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(
             task => {
                 if (!task.IsCanceled && !task.IsFaulted)
                 {
                     resultText.text = email + " 로 회원가입 하셨습니다.";
                 }
                 else
                 {
                     resultText.text = "회원가입에 실패하셨습니다.";
                 }
             }
         );
    }

    public void AutoGooglePlayGamesLogin()
    {
        googleText.text = "...";
        if (bWaitingForAuth)
            return;
        //구글 로그인이 되어있지 않다면 
        if (!Social.localUser.authenticated)
        {
            googleText.text = "Authenticating...";
            bWaitingForAuth = true;
            //로그인 인증 처리과정 (콜백함수)
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
                resultText.text = "로그인에 실패하였습니다.";
                return;
            }
            if (task.IsFaulted)
            {
                resultText.text = "SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception;
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            resultText.text = "로그인 성공" + result.User.DisplayName + ", " + result.User.UserId;
        });
    }

    // 수동로그인 
    public void OnBtnLoginClicked()
    {
        //이미 인증된 사용자는 바로 로그인 성공된다. 
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


    // 수동 로그아웃 
    public void OnBtnLogoutClicked()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        googleText.text = "LogOut...";
    }


    // 인증 callback
    void AuthenticateCallback(bool success)
    {
        googleText.text = "Loading";
        if (success)
        {
            // 사용자 이름을 띄어줌 
            googleText.text = "Welcome" + Social.localUser.userName + "\n";
            StartCoroutine(UserPictureLoad());
        }
        else
        {
            googleText.text = "Login Fail\n";
        }
    }

    // 유저 이미지 받아오기 
    IEnumerator UserPictureLoad()
    {
        googleText.text = "image Loading ...";
        // 최초 유저 이미지 가져오기
        Texture2D pic = Social.localUser.image;

        // 구글 아바타 이미지 여부를 확인 후 이미지 생성 
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
