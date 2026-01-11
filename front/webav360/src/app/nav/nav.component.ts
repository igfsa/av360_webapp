import { Component, inject, TemplateRef } from '@angular/core';

import { NgbTooltipModule, NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-nav',
  imports: [ NgbTooltipModule ],
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss', '../app.scss'],
})
export class NavComponent {
  private offcanvasService = inject(NgbOffcanvas);
	closeResult = '';

  // ngBootstrap function to open off canvas in the right side of screen
	openStart(content: TemplateRef<any>) {
		this.offcanvasService.open(content, { position: 'start' });
	}
}
