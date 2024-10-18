app.factory("colorCoding", function () {
    return {
        setBG: function (Occ, Ser) {

            var riskFactor;

            switch (parseInt(Occ)) {
                case 5:
                    switch (parseInt(Ser)) {
                        case 5:
                            riskFactor = "bgRed";
                            break;
                        case 4:
                            riskFactor = "bgRed";
                            break;
                        case 3:
                            riskFactor = "bgRed";
                            break;
                        case 2:
                            riskFactor = "bgRed";
                            break;
                        case 1:
                            riskFactor = "bgOrange";
                            break;
                    }
                    break;
                case 4:
                    switch (parseInt(Ser)) {
                        case 5:
                            riskFactor = "bgRed";
                            break;
                        case 4:
                            riskFactor = "bgRed";
                            break;
                        case 3:
                            riskFactor = "bgRed";
                            break;
                        case 2:
                            riskFactor = "bgOrange";
                            break;
                        case 1:
                            riskFactor = "bgOrange";
                            break;
                    }
                    break;
                case 3:
                    switch (parseInt(Ser)) {
                        case 5:
                            riskFactor = "bgRed";
                            break;
                        case 4:
                            riskFactor = "bgOrange";
                            break;
                        case 3:
                            riskFactor = "bgOrange";
                            break;
                        case 2:
                            riskFactor = "bgYellow";
                            break;
                        case 1:
                            riskFactor = "bgYellow";
                            break;
                    }
                    break;
                case 2:
                    switch (parseInt(Ser)) {
                        case 5:
                            riskFactor = "bgOrange";
                            break;
                        case 4:
                            riskFactor = "bgOrange";
                            break;
                        case 3:
                            riskFactor = "bgYellow";
                            break;
                        case 2:
                            riskFactor = "bgGreen";
                            break;
                        case 1:
                            riskFactor = "bgGreen";
                            break;
                    }
                    break;
                case 1:
                    switch (parseInt(Ser)) {
                        case 5:
                            riskFactor = "bgYellow";
                            break;
                        case 4:
                            riskFactor = "bgYellow";
                            break;
                        case 3:
                            riskFactor = "bgGreen";
                            break;
                        case 2:
                            riskFactor = "bgGreen";
                            break;
                        case 1:
                            riskFactor = "bgGreen";
                            break;
                    }
                    break;
            }

            return riskFactor;
        }
    }
});

app.factory("colorCodingForEAI", function () {
    return {
        setBG: function (risk) {

            var riskFactor;

            var riskF = Math.ceil(risk / parseFloat(125));
            if (parseInt(riskF) == 0) {
                riskF = 1;
            }
            switch (parseInt(riskF)) {
                case 25:
                case 24:
                case 23:
                case 22:
                case 21:
                case 20:
                case 19:
                case 18:
                case 17:
                case 16:
                    riskFactor = "bgRed";
                    break;
                case 15:
                case 14:
                case 13:
                case 12:
                case 11:
                    riskFactor = "bgOrange";
                    break;
                case 10:
                case 9:
                case 8:
                case 7:
                case 6:
                    riskFactor = "bgYellow";
                    break;
                case 5:
                case 4:
                case 3:
                case 2:
                case 1:
                    riskFactor = "bgGreen";
                    break;
            }

            return riskFactor;
        }
    }
});