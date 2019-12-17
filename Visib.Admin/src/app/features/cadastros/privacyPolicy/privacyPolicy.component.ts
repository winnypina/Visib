import { Component, OnInit } from '@angular/core';
import { ApiService } from '@app/core/services/api.service';
import { NotificationService } from '@app/core/services';

@Component({
    selector: 'sa-privacyPolicy',
    templateUrl: './privacyPolicy.component.html',
})
export class PrivacyPolicyComponent implements OnInit {

    currentLanguage:string;
    currentText:string;
    text: string = "Carregando..."
    textEnUs: string = "Loading..."
    textEs: string = "Carregando..."
    
    constructor(private apiService: ApiService,  private notificationService: NotificationService) {

    }

    ngOnInit() {
        this.currentLanguage = 'pt-BR';
        var self = this;
        this.apiService.get("PrivacyPolicy").subscribe(ret => {
            console.log(ret);
            self.text = ret.text;
            self.textEnUs = ret.textEnUs;
            self.textEs = ret.textEs;
            self.currentText = self.text;
        }, err => {
            console.log('Erro ao realizar requisição')
        });
    }

    onChange(language)
    {
        this.currentLanguage = language;
        if(language=='pt-BR')
        {
            this.currentText = this.text;
        }
        if(language=='en-US')
        {
            this.currentText = this.textEnUs;
        }
        if(language=='es-ES')
        {
            this.currentText = this.textEs;
        }
    }

    save()
    {
        var self = this;
        var current = { text: this.text,textEnUs: this.textEnUs,textEs: this.textEs };
        if(this.currentLanguage=='pt-BR')
        {
            this.text = this.currentText;
            current.text = this.currentText;
        }
        if(this.currentLanguage=='en-US')
        {
            this.textEnUs = this.currentText;
            current.textEnUs = this.currentText;
        }
        if(this.currentLanguage=='es-ES')
        {
            this.textEs = this.currentText;
            current.textEs = this.currentText;
        }

        this.apiService.post("PrivacyPolicy", current).subscribe(ret => {
            self.notificationService.smartMessageBox(
                {
                  title:
                    "<i class='fa fa-sign-out txt-color-orangeDark'></i> Registro salvo com sucesso! <span class='txt-color-orangeDark'></span>",
                  content:
                    "",
                  buttons: "[Ok]"
                }
              );
        }, err => {
            console.log(err)
        });
    }

}
