import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateDefaultParser, TranslateModule, TranslateService, TranslateStore } from '@ngx-translate/core';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { IconModule } from './components/icon/icon.module';

@NgModule({
    declarations: [
  ],
    imports: [CommonModule,
        TranslateModule,
        IconModule,
        ReactiveFormsModule,
        FormsModule
    ],
    providers: [
        provideHttpClient(withInterceptorsFromDi()),
        TranslateService,
        TranslateStore,
        TranslateDefaultParser,
    ],
    exports: [
        TranslateModule,
        IconModule,
        ReactiveFormsModule,
        ],
})
export class SharedModule { }
