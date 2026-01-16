import { Component, inject, TemplateRef  } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

import { NgbModal, NgbTooltipModule, NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports:  [
              RouterLink,
              RouterLinkActive,
              RouterOutlet
            ],
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss', '../../app.scss'],
})
export class NavComponent {
  private offcanvasService = inject(NgbOffcanvas);

	open(content: TemplateRef<any>) {
    setTimeout(() => this.offcanvasService.open(content));
	}
}
