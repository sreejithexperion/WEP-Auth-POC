import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CommunicationService {
  private valueSubject = new Subject<any>();
  value$ = this.valueSubject.asObservable();

  sendValue(value: any) {
    this.valueSubject.next(value);
  }
}