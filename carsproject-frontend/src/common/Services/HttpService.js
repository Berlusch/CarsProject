import axios from "axios";
import {PRODUKCIJA} from "../constants";

export const HttpService=axios.create({
    baseURL: PRODUKCIJA,
    headers:{
        'Content-Type':'application/json'
    }
});

