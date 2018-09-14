import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { FileSystemEntry } from './app.shared.module';

@Injectable()
export class DataService {

    private bsFolder = new BehaviorSubject<FileSystemEntry>(new FileSystemEntry() );
    currentFolder = this.bsFolder.asObservable();

    constructor() { }

    changeFolder(folder: FileSystemEntry) {
        this.bsFolder.next(folder);
    }

}