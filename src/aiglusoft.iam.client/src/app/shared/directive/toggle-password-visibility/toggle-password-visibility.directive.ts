import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
  selector: '[appTogglePasswordVisibility]'
})
export class TogglePasswordVisibilityDirective {

  constructor(private el: ElementRef) {}

  @HostListener('click') onClick() {
    const input = this.el.nativeElement.previousElementSibling;
    if (input && input.type === 'password') {
      input.type = 'text';
    } else if (input) {
      input.type = 'password';
    }
  }
}
