import { Component, ChangeDetectorRef, ViewChild, ViewEncapsulation, OnInit, OnDestroy, ElementRef } from '@angular/core';
import { MediaMatcher } from '@angular/cdk/layout';
import { Router, NavigationStart } from '@angular/router';
import { MatExpansionPanel, MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AccountService } from '@custor/services/security/account.service';
import { AuthService } from '@custor/services/security/auth.service';
import {  ConfigurationService } from '@custor/services/configuration.service';
import { Permission } from '@custor/models/permission.model';
import { AppTranslationService } from '@custor/services/translation.service';

@Component({
    selector: 'app-main',
    templateUrl: './main.component.html',
    styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit, OnDestroy {
    @ViewChild('customer') customerExpander: MatExpansionPanel;

    private _mobileQueryListener: () => void;
    isAppLoaded: boolean;
    isUserLoggedIn: boolean;
    isAdminExpanded = false;
    removePrebootScreen: boolean;
    newNotificationCount = 0;
    appTitle = 'OTRLS';
    appLogo = '';

    mobileQuery: MediaQueryList;

    constructor(private translationService: AppTranslationService,
                private accountService: AccountService,
                private authService: AuthService,
                public configService: ConfigurationService,
                public router: Router,
                changeDetectorRef: ChangeDetectorRef,
                media: MediaMatcher) {
        this.mobileQuery = media.matchMedia('(max-width: 600px)');
        this._mobileQueryListener = () => changeDetectorRef.detectChanges();
        this.mobileQuery.addListener(this._mobileQueryListener);

        // this.translationService.addLanguages(['en', 'et']);
        // this.configService.language = 'et';
        this.translationService.changeLanguage(this.configService.language);
    }

    ngOnInit() {
        this.isUserLoggedIn = true; // this.authService.isLoggedIn;
        // this.customerExpander.open();
        this.router.events.subscribe(event => {
            if (event instanceof NavigationStart) {
                const url = (event as NavigationStart).url;

                if (url !== url.toLowerCase()) {
                    this.router.navigateByUrl((event as NavigationStart).url.toLowerCase());
                }

                // if (this.adminExpander && url.indexOf('admin') > 0) {
                //     this.adminExpander.open();
                // }
            }
        });
    }

    ngOnDestroy() {
        this.mobileQuery.removeListener(this._mobileQueryListener);
    }


    logout() {
        this.authService.logout();
        this.authService.redirectLogoutUser();
    }

    get userName(): string {
        return this.authService.currentUser ? this.authService.currentUser.UserName : '';
    }

    get fullName(): string {
        return this.authService.currentUser ? this.authService.currentUser.FullName : '';
    }

    get canViewCustomers() {
        return this.accountService.userHasPermission(Permission.viewUsersPermission);
    }

    get canViewProducts() {
        return this.accountService.userHasPermission(Permission.viewUsersPermission);
    }

    get canViewOrders() {
        return true;
    }

    get canViewUsers() {
        return this.accountService.userHasPermission(Permission.viewUsersPermission);
    }

    get canViewRoles() {
        return this.accountService.userHasPermission(Permission.viewRolesPermission);
    }
}
