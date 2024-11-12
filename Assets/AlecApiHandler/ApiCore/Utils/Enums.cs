using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alec.Api
{
    public enum ResponseStatus
    {
        UNKNOWN_ERROR = 0,
        Successful = 1,
        INVALID_APP = 100,
        INVALID_VERSION = 101,
        INVALID_OTP = 103,
        INVALID_INPUT = 104,
        INVALID_PASSWORD = 105,
        USER_EXIST = 106,
        USER_DOES_NOT_EXIST = 107,
        DISCOUNT_VOUCHER_CODE_NOT_EXIST = 108,
        DISCOUNT_VOUCHER_CODE_TOTAL_USAGE = 109,
        DISCOUNT_VOUCHER_CODE_EXPIRED = 110,
        PACKAGE_HAS_DISCOUNT = 111,
        CREATE_PLAYER_ERR = 112,
        PLAYER_UPDATE_ERR = 113,
        UPDATE_USER_ERR = 114,
        DISCOUNT_VOUCHER_MORE_THAN_PACKAGE_PRICE_ERR = 115,
        CREATE_USER_ERR = 116,
        GET_USER_PLAYERS_ERR = 117,
        PACKAGE_DISCOUNT_ERR = 118,
        SHOW_USER_INFORMATION_ERR = 119,
        TRIAL_PACKAGE_ALREADY_ACTIVE = 120,
        PACKAGE_NOT_FOUND = 121,
        FCM_TOKEN_UPDATE_FAILED = 122,
        FCM_TOKEN_IS_EMPTY = 123,
        PLAYER_DOSES_NOT_EXIT = 124,
        OTP_LIMIT_TIME = 125,
        NOT_AUTHENTICATED = 401,
        DEVICE_NOT_FOUND = 404,
        SERVER_ERROR = 500,
        INTERNET_ERROR = 600,
        HTTP_ERROR = 700,
        MODEL_MISMATCH_ERR = 701
    }

    public enum ContentType
    {
        None,
        JSON,
        FormData
    }

    public enum MethodType
    {
        POST,
        GET,
        PUT
    }
}
