using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;



public static class Authentication {
    public static string PlayerId { get; private set; }

    public static async Task Login() {
        if (UnityServices.State == ServicesInitializationState.Uninitialized) {
            var options = new InitializationOptions();
            
            await UnityServices.InitializeAsync(options);
        }

        if (!AuthenticationService.Instance.IsSignedIn) {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            PlayerId = AuthenticationService.Instance.PlayerId;
        }
    }
}