import { Component } from '@angular/core';
import { AppTranslationService } from '@custor/services/translation.service';
import {ConfigurationService} from '@custor/services/configuration.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'otrls';
  constructor(private translationService: AppTranslationService,
              private configService: ConfigurationService) {
      this.translationService.addLanguages(['en', 'et']);
      this.configService.language = 'en';
      
  }
}
