package com.packageapp.exception;

public class InvalidPackageIdException extends Exception {
    public InvalidPackageIdException(String message) {
        super(message);
    }
}