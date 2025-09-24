import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoleStatusBadgeComponent } from './role-status-badge.component';

describe('RoleStatusBadgeComponent', () => {
  let component: RoleStatusBadgeComponent;
  let fixture: ComponentFixture<RoleStatusBadgeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RoleStatusBadgeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RoleStatusBadgeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
