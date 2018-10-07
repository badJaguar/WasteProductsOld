import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
/* Services */
import { BaseHttpService } from 'src/app/services/base/base-http.service';
import { LoggingService } from 'src/app/services/logging/logging.service';
import { OAuthService, JwksValidationHandler, AuthConfig } from 'angular-oauth2-oidc';
/* Environment */
import { environment } from 'src/environments/environment';

import { Registration } from '../models/registration';
import { Login } from '../models/login';


declare interface Claims {
  /**
     * Subject / Id
     */
  sub: string;
  /**
     * Email
     */
  email: string;
  /**
     * Is email verified
     */
  email_varified: string;
  /**
     * Full name
     */
  name: string;
  /**
     * Given name
     */
  given_name: string;
  /**
     * Family name
     */
  family_name: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService extends BaseHttpService {

  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);

  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(private oauthService: OAuthService, httpService: HttpClient, loggingService: LoggingService) {
    super(httpService, loggingService);
    this.configureOauth();
  }

  register(registrationModel: Registration): Observable<any> {
    const url = `${environment.apiHostUrl}/api/user/register`;  // URL to web api
    return this.httpService.post<any>(url, registrationModel);
  }

  logIn(  userModel? : Login): void {

    if (userModel) 
      this.oauthService.fetchTokenUsingPasswordFlowAndLoadUserProfile(userModel.UserName, userModel.Password)
      .then(() => {
      let claims = this.oauthService.getIdentityClaims();
      })
      //.then(()=>{this.oauthService.refreshToken()})
      ;

    else
    this.oauthService.initImplicitFlow();

  }

  logOut(): void {
    this.oauthService.logOut();
  }

  getProfile(): object {
    return this.oauthService.loadUserProfile();
  }

  getAccessToken(): string {
    return this.oauthService.getAccessToken();
  }

  getClaims(): Claims {
    return <Claims>this.oauthService.getIdentityClaims();
  }

  getUserId(): string {
  // const claims = this.getClaims();
  // if (claims) { return this.getClaims().sub; }
  //   return null;
    return '0';
  }

  isInScope(scope: string): boolean {
    const scopesString = (<Array<string>>this.oauthService.getGrantedScopes())[0];
    const scopes = scopesString.split(' ');
    return scopes.includes(scope);
  }

  private configureOauth() {

    // The SPA's id. Register SPA with this id at the auth-server
    this.oauthService.clientId = environment.clientId;
// set the scope for the permissions the client should request
    // The auth-server used here only returns a refresh token (see below), when the scope offline_access is requested
    this.oauthService.scope = environment.scope;
    // set storage for tokens
    this.oauthService.setStorage(sessionStorage);

    // Set a dummy secret
        // Please note that the auth-server used here demand the client to transmit a client secret, although
        // the standard explicitly cites that the password flow can also be used without it. Using a client secret
        // does not make sense for a SPA that runs in the browser. That's why the property is called dummyClientSecret
        // Using such a dummy secret is as safe as using no secret.
      this.oauthService.dummyClientSecret = environment.dummyClientSecret;
      this.oauthService.oidc = false;
      this.oauthService.issuer = environment.iderntityHostUrl;
      this.oauthService.showDebugInformation = !environment.production;
      this.oauthService.sessionChecksEnabled = true;

      ////this.oauthService.responseType = 'token';
      // URL of the SPA to redirect the user to after login
      //this.oauthService.redirectUri =  'http://localhost:4200/login';
      // URL of the SPA to redirect the user after silent refresh
      //this.oauthService.silentRefreshRedirectUri= 'http://localhost:4200/silent-refresh.html'; //silent-refresh
      //this.oauthService.postLogoutRedirectUri = 'http://localhost:4200';

    //use for implicit flow  
    // set AuthConfig
    //this.oauthService.configure(this.createAuthConfig());
    // set automatic refresh
     this.oauthService.setupAutomaticSilentRefresh();
    // set token validation handler
     this.oauthService.tokenValidationHandler = new JwksValidationHandler();

    // subscribe to login/logout events
    this.oauthService.events.subscribe(event => {
      const oldValue = this.isAuthenticatedSubject.value;
      const newValue = this.oauthService.hasValidIdToken() && this.oauthService.hasValidAccessToken();
      if (oldValue !== newValue) {
        this.isAuthenticatedSubject.next(newValue);
      }
    });

  
     // Load Discovery Document and then try to login the user
     this.oauthService.loadDiscoveryDocument(environment.urlDiscoveryDocument).then(() => {
      this.logDebug('Connection to the IdentityServer established and Discovery Document was successfully loaded');
      // try to login
      this.oauthService.tryLogin({
        onTokenReceived: () =>
          this.logDebug('LogIn completed'),
        onLoginError: () =>
          this.logError('LogIn error')
      });
    })
    .catch(() => this.logError('The AuthenticationService can not connect to the IdentityServer or load DiscoveryDocument from it'))
  }

  private createAuthConfig(): AuthConfig {
    return {
      oidc:  false,
      //not use in password flow with discoverydocument
      issuer: environment.iderntityHostUrl, // Url of the Identity Provider
      
      clientId: environment.clientId, // The SPA's id. The SPA is registerd with this id at the auth-server

      //not use in password flow
      redirectUri: window.location.origin, // URL of the SPA to redirect the user to after login
      
      scope: 'openid profile email wasteproducts-api', // set the scope for the permissions the client should request

      showDebugInformation: !environment.production,
      sessionChecksEnabled: true,

    };
  }
}
