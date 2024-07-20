import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

@Component({
  selector: 'app-root',
  template: `<router-outlet/>`
})
export class AppComponent implements OnInit {
  
  constructor(private http: HttpClient) {}

  ngOnInit() {
   
  }


  title = 'aiglusoft.iam.client';
}
