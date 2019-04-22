import { Injectable } from '@angular/core';

@Injectable()
export class Region {
    public RegionId: string;
    public Description: string;
    public DescriptionEnglish: string;

  }

@Injectable()
export class Zone {
    public ZoneId: string;
    public isNew: boolean;
    public RegionId: string;
    public Description: string;
    public DescriptionEnglish: string;
    public Region?: Region;

  }

export class Woreda {
    public RegionId: string;
    public ZoneId: string;
    public WoredaId: string;
    public isNew: boolean;
    public Description: string;
    public DescriptionEnglish: string;
    public Region: Region;
    public Zone: Zone;

  }

@Injectable()
export class Kebele {
    public RegionId: string;
    public ZoneId: string;
    public KebeleId: string;
    public WoredaId: string;
    public Description: string;
    public DescriptionEnglish: string;
    public Region: Region;
    public Zone: Zone;
    public Woreda: Woreda;
  }

