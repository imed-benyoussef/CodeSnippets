import {
  Directive,
  ElementRef,
  Input,
  OnChanges,
  Renderer2,
  SimpleChanges,
} from '@angular/core';

@Directive({
  selector: '[asuiIcon]',
})
export class IconDirective implements OnChanges {
  @Input() asuiIcon!: string; // Consistent with selector

  private currentIconClass: string | null = null; // To store the current icon class

  constructor(
    private el: ElementRef,
    private renderer: Renderer2
  ) {}

  ngOnChanges(changes: SimpleChanges) {
    // Check if asuiIcon has changed
    if (changes['asuiIcon']) {
      this.updateIconClass();
    }
  }

  private updateIconClass() {
    // If a previous icon class exists, remove it
    if (this.currentIconClass) {
      this.renderer.removeClass(this.el.nativeElement, this.currentIconClass);
    }

    // If a new asuiIcon value exists, add the new class
    if (this.asuiIcon) {
      this.currentIconClass = `icon-${this.asuiIcon}`;
      this.renderer.addClass(this.el.nativeElement, 'icon');
      this.renderer.addClass(this.el.nativeElement, this.currentIconClass);
    } else {
      this.currentIconClass = null; // Reset when asuiIcon is empty or null
    }
  }
}
