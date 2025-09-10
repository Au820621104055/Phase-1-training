import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RestaurantService } from 'src/app/shared/services/restaurant.service';
import { Restaurant } from 'src/app/shared/models/restaurant.interface';

@Component({
  selector: 'app-restaurant-list',
  templateUrl: './restaurant-list.component.html',
  styleUrls: ['./restaurant-list.component.css']
})
export class RestaurantListComponent implements OnInit {
  restaurants: Restaurant[] = [];          
  filteredRestaurants: Restaurant[] = []; 
  searchTerm: string = '';                  
  loading = false;
  error = '';

  constructor(private restaurantService: RestaurantService, private router: Router) {}

  ngOnInit(): void {
    this.fetchRestaurants();
  }

  fetchRestaurants(): void {
    this.loading = true;
    this.restaurantService.getAllRestaurants().subscribe({
      next: (data) => {
        this.restaurants = data.map((r, index) => ({
          ...r,
          image: `/assets/images/res${index + 1}.jpg` 
        }));
        this.filteredRestaurants = [...this.restaurants]; 
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load restaurants';
        this.loading = false;
      }
    });
  }

  onSearchChange(value: string): void {
    const term = value.toLowerCase().trim();

    this.filteredRestaurants = this.restaurants.filter(r =>
      r.name.toLowerCase().includes(term) ||
      (r.cuisineType && r.cuisineType.toLowerCase().includes(term))
    );
  }

  viewDetails(id: number): void {
    this.router.navigate(['/customer/restaurant-details', id]);
  }

  goToCart(): void {
    this.router.navigate(['/customer/cart']);
  }
}
