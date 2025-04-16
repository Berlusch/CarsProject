import axios from "axios";
import { Backend_URL } from "../constants";

export const HttpService=axios.create({
    baseURL:Backend_URL,
    headers:{
        'Content-Type':'application/json'
    }
});


