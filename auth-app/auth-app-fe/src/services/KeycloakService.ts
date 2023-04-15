import Keycloak from "keycloak-js";
import HttpService from "./HttpService";

const keycloakInstance = new Keycloak();

const Login = (onAuthenticatedCallback: Function) => {
  keycloakInstance
    .init({ onLoad: "login-required" })
    .then(function (authenticated) {
      authenticated ? onAuthenticatedCallback() : alert("non authenticated");
      updateToken(HttpService.configure(keycloakInstance.token));
    })
    .catch((e) => {
      console.dir(e);
      console.log(`keycloak init exception: ${e}`);
    });
}

const UserName = () => keycloakInstance.tokenParsed?.preferred_username;

const UserRole = () => keycloakInstance.resourceAccess?.['my-app']?.roles;
const doLogin = keycloakInstance.login;

const updateToken = (successCallback: any) => keycloakInstance.updateToken(5).then(successCallback).catch(doLogin);

const Logout = keycloakInstance.logout;

const KeyCloakService = {
  CallLogin: Login,
  GetUserName: UserName,
  GetUserRoles: UserRole,
  CallLogout: Logout,
  IsLoggedIn: !!keycloakInstance.token,
  GetToken: () => {
    console.log(keycloakInstance)
    return keycloakInstance.token
  },
  UpdateToken: updateToken,
};

export default KeyCloakService;
