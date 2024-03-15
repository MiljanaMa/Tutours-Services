import { TestBed } from '@angular/core/testing';

import { MarketplaceService } from './marketplace.service';

describe('MarketplaceService', () => {
  let service: MarketplaceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MarketplaceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
