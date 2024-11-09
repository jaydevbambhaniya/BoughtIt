import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthCompleteComponent } from './auth-complete.component';

describe('AuthCompleteComponent', () => {
  let component: AuthCompleteComponent;
  let fixture: ComponentFixture<AuthCompleteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuthCompleteComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AuthCompleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
