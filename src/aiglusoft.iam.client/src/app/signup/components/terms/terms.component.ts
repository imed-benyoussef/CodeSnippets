import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { TermsService } from '../../services/terms.service';

@Component({
  selector: 'app-terms',
  template: `
    <div class="terms-content mt-4" [formGroup]="formGroup">
      <div class="form-check">
        <input type="checkbox" class="form-check-input" id="acceptTerms" formControlName="termsAccepted">
        <label class="form-check-label" for="acceptTerms">
          {{ 'signup.components.terms.accept' | translate }}
          <br>
          <a href="javascript:void(0);" (click)="openTermsModal()">{{ 'signup.components.terms.read' | translate }}</a>
        </label>
      </div>
      <div class="invalid-feedback" *ngIf="formGroup.get('termsAccepted')?.invalid && formGroup.get('termsAccepted')?.touched">
        {{ 'signup.components.terms.validation.required' | translate }}
      </div>
    </div>

    <!-- Terms Modal -->
    <div class="modal fade" id="termsModal" tabindex="-1" aria-labelledby="termsModalLabel" aria-hidden="true">
      <div class="modal-dialog modal-dialog-centered modal-lg">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="termsModalLabel">{{ 'signup.components.terms.modalTitle' | translate }}</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body" style="max-height: 60vh; overflow-y: auto;" (scroll)="checkScroll($event)">
            <div [innerHTML]="termsContent"></div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-link" data-bs-dismiss="modal">{{ 'signup.common.close' | translate }}</button>
            <button type="button" class="btn btn-primary" (click)="acceptTerms()" [disabled]="!fullyScrolled">{{ 'signup.common.accept' | translate }}</button>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    @media (min-width: 992px) {
      .modal-lg {
        max-width: 60%;
      }
    }
  `]
})
export class TermsComponent implements OnInit {
  @Input() formGroup!: FormGroup;
  @Output() termsAccepted = new EventEmitter<void>();
  termsContent: string = '';
  fullyScrolled = false;

  constructor(private termsService: TermsService) {}

  ngOnInit() {
    this.loadTerms();
  }

  loadTerms() {
    this.termsService.getTerms().subscribe(
      (data: string) => {
        this.termsContent = data;
      },
      (error) => {
        console.error('Error loading terms and conditions:', error);
      }
    );
  }

  openTermsModal() {
    const modalElement = document.getElementById('termsModal');
    if (modalElement) {
      //const modal = new bootstrap.Modal(modalElement);
      //modal.show();
      alert("openTermsModal");
    }
  }

  checkScroll(event: Event) {
    const element = event.target as HTMLElement;
    this.fullyScrolled = element.scrollTop + element.clientHeight >= element.scrollHeight;
  }

  acceptTerms() {
    this.formGroup.get('termsAccepted')?.setValue(true);
    this.termsAccepted.emit();
    const modalElement = document.getElementById('termsModal');
    if (modalElement) {
      //const modal = bootstrap.Modal.getInstance(modalElement);
      //modal?.hide();

      alert("acceptTerms");
    }
  }
}
