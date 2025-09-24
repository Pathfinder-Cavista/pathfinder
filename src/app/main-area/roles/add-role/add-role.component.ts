import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Editor, NgxEditorModule } from 'ngx-editor';
import { AlertService } from '../../../../generated/services/alert/alert.service';
import { Subscription } from 'rxjs';
import { RoleService } from '../../../../generated/services/role/role.service';

@Component({
  selector: 'app-add-role',
  imports: [
    NgxEditorModule,
    ReactiveFormsModule,
    FormsModule,
  ],
  templateUrl: './add-role.component.html',
  styleUrl: './add-role.component.scss'
})
export class AddRoleComponent implements OnInit, OnDestroy {

  protected editor!: Editor;
  protected descriptionText: string = '';
  protected skillsText: string = '';
  protected employmentText: string = '';
  protected levelText: string = '';
  protected addRoleForm = new FormGroup({
    title: new FormControl('', [Validators.required]),
    description: new FormControl('', [Validators.required]),
    deadLine: new FormControl('', [Validators.required]),
    requirements: new FormControl([''], [Validators.required]),
    publishNow: new FormControl(true, [Validators.required]),
    employmentType: new FormControl(-1, [Validators.required, Validators.min(0), Validators.max(4)]) as FormControl<number>,
    level: new FormControl(-1, [Validators.required, Validators.min(0), Validators.max(6)]) as FormControl<number>,
    location: new FormControl('', [Validators.required]),
    employmentTypeS: new FormControl('-1', [Validators.required, Validators.min(0), Validators.max(4)]) as FormControl<string>,
    levelS: new FormControl('-1', [Validators.required, Validators.min(0), Validators.max(6)]) as FormControl<string>,
    skills: new FormControl([''], [Validators.required]),
  });

  private unsubscribe: Subscription[] = [];

  constructor(private alertService: AlertService, private roleService: RoleService) {}

  protected submitNewRole() {
    this.addRoleForm.controls.description.setValue(this.descriptionText);
    this.addRoleForm.controls.skills.setValue(this.skillsText.split(',').map(x => x.trim()));
    this.addRoleForm.controls.employmentType.setValue(Number.parseInt(this.addRoleForm.controls.employmentTypeS.value));
    this.addRoleForm.controls.level.setValue(Number.parseInt(this.addRoleForm.controls.levelS.value));

    if (this.addRoleForm.invalid) {
      this.alertService.showDanger('Error', 'The form has some missing data');
      return;
    }

    this.alertService.showInfo('Please Wait', 'Adding the new role...');
    
    const sub = this.roleService.addRole(this.addRoleForm.value).subscribe({
      next: () => {
        this.alertService.showSuccess('Success', 'Added new role successfully!');
        this.addRoleForm.reset();
        this.descriptionText = this.skillsText = ''
      },
      error: err => {
        this.alertService.showDanger('Error', err.error.message);
      }
    });

    this.unsubscribe.push(sub);
  }

  ngOnInit(): void {
    this.editor = new Editor();
  }

  ngOnDestroy(): void {
    this.editor.destroy();
  }
}
