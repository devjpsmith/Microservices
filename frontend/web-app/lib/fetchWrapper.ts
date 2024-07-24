const baseUrl = 'http://localhost:6001/';
type Method = 'GET' | 'PUT' | 'POST' | 'DELETE';

type Headers = {
    'Content-Type': string;
    Authorization?: string;
};

type RequestOptions = {
    method: Method;
    headers: Headers;
    body?: string;
};

function getHeaders(token?: string) {
    const headers: Headers = { 'Content-Type': 'application/json' };
    if (token) headers.Authorization = `Bearer ${token}`;
    return headers;
}

function get(url: string) {
    return request('GET', url, null);
}

function post(url: string, body: {}, token?: string) {
    return request('POST', url, body, token);
}

async function put(url: string, body: {}, token?: string) {
    return request('PUT', url, body, token);
}

function del(url: string, token?: string) {
    return request('DELETE', url, null, token);
}

async function request(method: Method, url: string, body: {} | null, token?:string) {
    const requestOptions: RequestOptions = {
        method: method,
        headers: getHeaders(token)
    };
    if (body) requestOptions.body = JSON.stringify(body)
    const response = await fetch(baseUrl + url, requestOptions);
    return await handleResponse(response);
}

async function handleResponse(response: Response) {
    const text = await response.text();
    const data = text && JSON.parse(text);

    if (response.ok) {
        return data || response.statusText;
    } else {
        const error = {
            status: response.status,
            message: response.statusText,
        };
        return {error};
    }
}

export const fetchWrapper = {
    get,
    post,
    put,
    del
};
