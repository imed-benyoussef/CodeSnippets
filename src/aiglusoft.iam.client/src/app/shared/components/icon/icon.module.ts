import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IconDirective } from './icon.directive';
import { IconsComponent } from './icons.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [IconDirective, IconsComponent],
  imports: [CommonModule, FormsModule],
  exports: [IconDirective, IconsComponent],
})
export class IconModule {}
