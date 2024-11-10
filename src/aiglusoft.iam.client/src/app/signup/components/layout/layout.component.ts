import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-layout',
  template: `
  
  <div class="sm:mx-auto sm:w-full sm:max-w-md">
      <h4 class="fw-normal" [translate]="title" [translateParams]="params"></h4>
      <p class="mt-3 text-muted mb-0" *ngIf="instructions" [translate]="instructions" [translateParams]="params"></p>
    </div>
    <br class="m-5">
    <ng-content></ng-content>
  `
})
export class LayoutComponent {
  @Input() title!: string;
  @Input() instructions: string | undefined;
  @Input() params: any;
}
