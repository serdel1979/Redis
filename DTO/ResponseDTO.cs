﻿namespace Redis.DTO
{
    public class ResponseDTO<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }
}
