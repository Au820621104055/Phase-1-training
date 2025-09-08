import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../shared/services/admin.service';
import { User } from '../../shared/models/user.interface';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {

  users: User[] = [];
  userForm: FormGroup;
  editingUserId: number | null = null;

  constructor(private adminService: AdminService, private fb: FormBuilder) {
    this.userForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      role: ['Customer', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.adminService.getAllUsers().subscribe(users => this.users = users);
  }

  addOrUpdateUser(): void {
    const userData = this.userForm.value;

    if (this.editingUserId) {
      this.adminService.updateUser(this.editingUserId, userData).subscribe(() => {
        this.loadUsers();
        this.userForm.reset({ role: 'Customer' });
        this.editingUserId = null;
      });
    } else {
      this.adminService.addUser(userData).subscribe(() => {
        this.loadUsers();
        this.userForm.reset({ role: 'Customer' });
      });
    }
  }

  editUser(user: User): void {
    this.editingUserId = user.userId;
    this.userForm.setValue({
      name: user.fullName,
      email: user.email,
      role: user.role
    });
  }

  deleteUser(userId: number): void {
    if (!confirm('Are you sure you want to delete this user?')) return;
    this.adminService.deleteUser(userId).subscribe(() => this.loadUsers());
  }
}
