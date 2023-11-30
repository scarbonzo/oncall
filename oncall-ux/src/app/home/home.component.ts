import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { OncallService } from '../oncall.service';
import { Oncall } from '../oncall';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  providers: [DatePipe]
})
export class HomeComponent implements OnInit {
  coreTeam = [ "James", "Rich", "Dan", "Eric", "Tim", "Raj" ];
  webTeam = [ "Sri", "Corrado", "Pedro", "Mikhail", "Mark" ];
  years = [ 2023, 2024, 2025 ];
  weeks: number[] = Array.from({ length: 52 }, (_, i) => i + 1);

  currentYear: any;
  currentWeek: any;
  selectedYear: any;
  selectedWeek: any;
  
  core: any;
  web: any;

  constructor(private oncallService: OncallService, private datePipe: DatePipe) {
  }

  ngOnInit(): void {
    this.currentYear = this.datePipe.transform(new Date(), 'yyyy');
    this.currentWeek = this.datePipe.transform(new Date(), 'ww');
    this.selectedWeek = this.currentWeek;
    this.selectedYear = new Date().getFullYear();
    this.UpdateWeek();
  }

  UpdateWeek() {
    this.oncallService
      .getOncall('Core', this.selectedYear, this.selectedWeek)
      .subscribe(data => {  this.core = data;
    });

    this.oncallService
      .getOncall('Web', this.selectedYear, this.selectedWeek)
      .subscribe(data => {  this.web = data;
    });
  }
}
