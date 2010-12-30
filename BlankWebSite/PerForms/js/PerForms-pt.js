function performs_Translate(key) {
    key = performs_SplitTokenString(key);
    switch (key[0]) {
        case "[RequiredField]": return "Este campo é obrigatório.";
        case "[ERROR]": return "Erro";
        case "[INFO]": return "Informação";
        case "[PleaseWait]": return "Por favor, aguarde...";
        case "[ExceptionErrorMessage]": return "Ocorreu um imprevisto na aplicação.<br />A equipa foi notificada e irá verificar assim que possível.<br />Se desejar entrar em contacto com a equipa, indique por favor a seguinte chave: <strong>" + key[1] + "</strong>";
        case "[GenericErrorMessage]": return "Ocorreu um imprevisto na aplicação.<br />A equipa foi notificada e irá verificar assim que possível.<br />";
        case "[SessionExpired]": return "A sua sessão expirou. Por favor, faça login novamente.";
        case "[ChooseFile]": return "Escolher ficheiro...";
        case "[DropFileHere]": return "- large o ficheiro aqui -";
        case "[Cancel]": return "Cancelar";
        case "[Date_Format_Long]": return "dd 'de' MM 'de' yy";
        case "[Date_Format_Short]": return "dd-mm-yy";
        case "[January]": return "Janeiro";
        case "[February]": return "Fevereiro";
        case "[March]": return "Março";
        case "[April]": return "Abril";
        case "[May]": return "Maio";
        case "[June]": return "Junho";
        case "[July]": return "Julho";
        case "[August]": return "Agosto";
        case "[September]": return "Setembro";
        case "[October]": return "Outubro";
        case "[November]": return "Novembro";
        case "[December]": return "Dezembro";
        case "[Sunday_Long]": return "Domingo";
        case "[Monday_Long]": return "Segunda";
        case "[Tuesday_Long]": return "Terça";
        case "[Wednesday_Long]": return "Quarta";
        case "[Thursday_Long]": return "Quinta";
        case "[Friday_Long]": return "Sexta";
        case "[Saturday_Long]": return "Sábado";
        case "[Sunday_Short]": return "D";
        case "[Monday_Short]": return "S";
        case "[Tuesday_Short]": return "T";
        case "[Wednesday_Short]": return "Q";
        case "[Thursday_Short]": return "Q";
        case "[Friday_Short]": return "S";
        case "[Saturday_Short]": return "S";
        case "[ExportToExcel]": return "Exportar para Excel";
        default: return key[0];
    }
}