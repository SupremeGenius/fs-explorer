import { Component, Inject, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { DataService } from "../../data.service";
import { FileSystemEntry } from '../../app.shared.module';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {

    currentFolder: FileSystemEntry | null;

    currentFolderContent: FileSystemEntry[];

    isLoading: boolean;

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private dataService: DataService) {
        this.isLoading = false;
        this.currentFolder = null;
    }

    onSelectFolder(folder: FileSystemEntry): void {
        this.dataService.changeFolder(folder);
    }

    ngOnInit() {
        this.dataService.currentFolder.subscribe(currentFolder => {
            this.isLoading = true;
            this.currentFolder = currentFolder;
            var thisFolder = this.currentFolder;
            if (this.currentFolder && this.currentFolder.fullPath) {
                this.http.get(this.baseUrl + 'api/FileSystem/GetFolderContent?path=' + encodeURIComponent(this.currentFolder.fullPath)).subscribe(result => {
                    this.currentFolderContent = result.json() as FileSystemEntry[];
                    this.currentFolder = thisFolder;
                    this.isLoading = false;
                }, error => {
                    console.error(error);
                    this.isLoading = false;
                });
            }
        });
    }
}
