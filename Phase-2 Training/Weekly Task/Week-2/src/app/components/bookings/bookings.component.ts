import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-bookings',
  standalone: true,
  imports: [CommonModule],   
  templateUrl: './bookings.component.html',
  styleUrls: ['./bookings.component.css']
})
export class BookingsComponent {
  bookings = [
    { movie: 'Coolie', name: 'Narendra', seats: 2, date: '2025-09-01', time: '7:00 PM' },
    { movie: 'Coolie', name: 'Arun', seats: 4, date: '2025-09-02', time: '9:00 PM' },
    { movie: 'Coolie', name: 'Priya', seats: 1, date: '2025-09-03', time: '6:00 PM' }
  ];
}
