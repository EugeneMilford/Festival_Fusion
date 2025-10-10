import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddFestivalComponent } from './add-festival.component';

describe('AddFestivalComponent', () => {
  let component: AddFestivalComponent;
  let fixture: ComponentFixture<AddFestivalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddFestivalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddFestivalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
