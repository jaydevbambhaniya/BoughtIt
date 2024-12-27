const ErrorCodesInternal: { [key: string]: { message: string } } = {
    "-1": {
      message: 'User not found, Please check the entered details.'
    },
    "-2": {
      message: 'Invalid credentials, Please try again.'
    },
    "-3": {
      message: 'Old password is incorrect!'
    },
    "-4": {
      message: 'Invalid Access token or Refresh token.'
    },
    "-5": {
      message: 'Invalid Refresh token.'
    },
    "-6": {
      message: 'Order Not Found.'
    },
    "-100": {
      message: 'Something went wrong, Please try again later.'
    },
  };
  
  export const ErrorCodes = new Proxy(ErrorCodesInternal, {
    get(target, prop: string) {
      if (prop in target) {
        return target[prop];
      }
      return { message: 'An unexpected error occurred. Please try again later.' };
    }
  });
  