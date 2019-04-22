import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { MainComponent } from './main.component';
import { SharedModule } from '@custor/modules/shared.module';
import { RouterModule } from '@angular/router';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import {AppTranslationService, TranslateLanguageLoader} from '@custor/services/translation.service';
// import {ConfigurationService} from '@custor/services/configuration.service';
import {EndpointFactory} from '@custor/services/security/endpoint-factory.service';
import {AccountService} from '@custor/services/security/account.service';
import {AccountEndpoint} from '@custor/services/security/account-endpoint.service';
// import { LocalStoreManager } from '@custor/services/storeManager.service';
import {AuthService} from '@custor/services/security/auth.service';
import {LangSwitcherModule} from '@custor/components/lang-switcher/lang-switcher.component';


export const routes = [
      {
            path: '',
            component: MainComponent, children: [
                { path: 'dashboard', loadChildren: './dashboard/dashboard.module#DashboardModule'},
                { path: 'customer', loadChildren: './customer/customer.module#CustomerModule'}
            ]
    }
  ];

 
@NgModule({
  declarations: [
    // GroupByPipe,
    MainComponent,
    
   ],
  imports: [
    HttpClientModule,
    ToastrModule.forRoot(),
    SharedModule,
    RouterModule.forChild(routes),
    LangSwitcherModule,
    TranslateModule.forRoot({
        loader: {
          provide: TranslateLoader,
          useClass: TranslateLanguageLoader
        }
      }),
   ],
   providers: [
    AppTranslationService,
    // ConfigurationService,
    AccountService,
    AccountEndpoint,
    EndpointFactory,
    // LocalStoreManager, 
    AuthService
  ],
})
export class MainModule { }
