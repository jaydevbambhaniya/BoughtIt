import { TestBed } from '@angular/core/testing';

import { BrowserstorageserverService } from './browserstorageserver.service';

describe('BrowserstorageserverService', () => {
  let service: BrowserstorageserverService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BrowserstorageserverService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
