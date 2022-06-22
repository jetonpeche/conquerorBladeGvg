import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-confirmation',
  templateUrl: './confirmation.component.html',
  styleUrls: ['./confirmation.component.scss']
})
export class ConfirmationComponent implements OnInit , OnDestroy
{
  protected titre: string = "";
  protected message: string = "";

  constructor(
    @Inject(MAT_DIALOG_DATA) private data: any,
    private dialogRef: MatDialogRef<ConfirmationComponent>
    ) { }

  ngOnInit(): void
  {
    this.titre = this.data.titre;
    this.message = this.data.message;
  }
  ngOnDestroy(): void 
  {
    this.dialogRef.close(false);
  }

}
