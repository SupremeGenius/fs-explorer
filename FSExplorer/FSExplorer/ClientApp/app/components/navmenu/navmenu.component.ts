import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { FSTreeNodeComponent } from '../fstreenode/fstreenode.component';
import { FileSystemEntry } from '../../app.shared.module'

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent {

    public systemDrives: FileSystemEntry[];

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        http.get(baseUrl + 'api/FileSystem/GetDrives').subscribe(result => {
            this.systemDrives = result.json() as FileSystemEntry[];
        }, error => console.error(error));
    }
}
