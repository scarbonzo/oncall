import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class OncallService {

  private baseUrl = 'http://192.168.100.120:8100/api/oncall/';

  constructor(private httpClient : HttpClient) { }

  getOncall(team: string, year: number, week: number) {
    return this.httpClient.get(this.baseUrl + 'team/' + team + '/' + year + '/' + week + '/');
  }

  createOncall(team: string, year: number, week: number, primary: string, backup: string) {
    return this.httpClient.get(this.baseUrl + 'create/' + team + '/' + year + '/' + week + '/' + primary  + '/' + backup);
  }

  updateOncall(team: string, year: number, week: number, primary: string, backup: string) {
    return this.httpClient.get(this.baseUrl + 'update/' + team + '/' + year + '/' + week + '/' + primary  + '/' + backup);
  }

  deleteOncall(team: string, year: number, week: number) {
    return this.httpClient.get(this.baseUrl + 'delete/' + team + '/' + year + '/' + week + '/');
  }
}
