import {Injectable, Injector} from '@angular/core';
import {StaticData} from '../models/static-data.model';
import {HttpClient} from '@angular/common/http';
import {catchError, map} from 'rxjs/operators';
import {Observable} from 'rxjs';
import { ConfigurationService } from '@custor/services/configuration.service';
import { EndpointFactory } from '@custor/services/security/endpoint-factory.service';

@Injectable()
export class LookUpService extends EndpointFactory  {
  private readonly _lookupByParentUrl: string = 'api/Lookup';

  constructor(private httpClient: HttpClient,
              private config: ConfigurationService,
              injector: Injector) {
      super(httpClient, config, injector);
}

get lookupByParentUrl() {return this.config.baseUrl + this._lookupByParentUrl; }

getLookupByParentId(lang: string, id: string): Observable<StaticData[]> {
    const endpointUrl = `${this.lookupByParentUrl}/${lang}/${id}`;
    return this.httpClient.get<StaticData[]>(endpointUrl, this.getRequestHeaders()).pipe(
      map(result => {
        return result;
      }));
}
}
