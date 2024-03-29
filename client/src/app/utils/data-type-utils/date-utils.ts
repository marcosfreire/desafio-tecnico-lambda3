import { IMyOptions } from "mydatepicker";

export class DateUtils {

    public static convertUTCDateToLocalDate(date): Date {
        var newDate = new Date(date.getTime() + date.getTimezoneOffset() * 60 * 1000);

        var offset = date.getTimezoneOffset() / 60;
        var hours = date.getHours();

        newDate.setHours(hours - offset);

        return newDate;
    }

    public static setMyDatePickerDate(myDate: any): Object {
        let pickerDate = new Date(myDate);
        return { date: { year: pickerDate.getFullYear(), month: pickerDate.getMonth() + 1, day: pickerDate.getDate() } };
    }

    public static getMyDatePickerDate(myDate: any): Date {
        if(!myDate) return undefined;
        return new Date(myDate.date.year, myDate.date.month - 1, myDate.date.day);
    }

    public static getMyDatePickerOptions(): IMyOptions {
        let dateNow = this.convertUTCDateToLocalDate(new Date());
        let myDatePickerOptions: IMyOptions = {
            selectionTxtFontSize: '14px',
            dateFormat: 'dd/mm/yyyy',
            dayLabels: { su: 'Dom', mo: 'Seg', tu: 'Ter', we: 'Qua', th: 'Qui', fr: 'Sex', sa: 'Sab' },
            monthLabels: { 1: 'Jan', 2: 'Fev', 3: 'Mar', 4: 'Abr', 5: 'Mai', 6: 'Jun', 7: 'Jul', 8: 'Ago', 9: 'Set', 10: 'Out', 11: 'Nov', 12: 'Dez' },
            showTodayBtn: false,
            firstDayOfWeek: "mo",
            markCurrentDay: true,
            minYear: dateNow.getFullYear(),
            maxYear: dateNow.getFullYear() + 3,
            // disableSince:{ year: dateNow.getFullYear(), month: dateNow.getUTCMonth() + 1, day: dateNow.getDate() },            
            height: '34px',
            width: '350px',
            editableDateField:true
        };

        return myDatePickerOptions;
    }
}
