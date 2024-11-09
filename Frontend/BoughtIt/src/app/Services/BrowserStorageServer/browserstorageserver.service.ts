import { Injectable } from '@angular/core';
import { BrowserStorageService } from '../BrowserStorage/browserstorage.service';

@Injectable({
  providedIn: 'root'
})
export class BrowserstorageserverService extends BrowserStorageService {

  constructor() { 
    super({
      clear: () => {},
      getItem: (key: string) => JSON.stringify({ key }),
      setItem: (key: string, value: string) => JSON.stringify({ [key]: value }),
      key: (index: number) => index.toString(),
      length: 0,
      removeItem: (key: string) => JSON.stringify({ key }),
    });
  }
}
