/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { SessaoService } from './Sessao.service';

describe('Service: Sessao', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SessaoService]
    });
  });

  it('should ...', inject([SessaoService], (service: SessaoService) => {
    expect(service).toBeTruthy();
  }));
});
