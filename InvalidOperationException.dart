library ExceptionExt

class InvalidOperationException implements Exception {
    String cause;
    InvalidOperationException(this.cause);
}