export const cartInitialState = []

export const CART_ACTION_TYPES = {
    ADD_TO_CART: 'ADD_TO_CART',
    REMOVE_FROM_CART: 'REMOVE_FROM_CART',
    UPDATE_QUANTITY: 'UPDATE_QUANTITY',
    CLEAR_CART: 'CLEAR_CART'
}

export const cartReducer = (state, action) => {
    const { type: actionType, payload: actionPayload } = action

    switch (actionType) {
        case CART_ACTION_TYPES.ADD_TO_CART: {
            const { id } = actionPayload
            const productInCartIndex = state.findIndex(item => item.id === id)

            if (productInCartIndex >= 0) {
                // Producto ya está en el carrito, incrementamos la cantidad por defecto en 1 aunque se puede setear
                const newState = structuredClone(state)
                newState[productInCartIndex].quantity += 1
                return newState
            }

            // Producto no está, se añade con cantidad 1
            return [
                ...state,
                {
                    ...actionPayload,
                    quantity: 1
                }
            ]
        }

        case CART_ACTION_TYPES.REMOVE_FROM_CART: {
            const { id } = actionPayload
            return state.filter(item => item.id !== id)
        }

        case CART_ACTION_TYPES.UPDATE_QUANTITY: {
            const { id, quantity } = actionPayload
            const productInCartIndex = state.findIndex(item => item.id === id)

            if (productInCartIndex >= 0) {
                const newState = structuredClone(state)
                // Asegurarse de que la cantidad sea al menos 1
                const newQuantity = Math.max(1, parseInt(quantity) || 1)
                newState[productInCartIndex].quantity = newQuantity
                return newState
            }
            return state
        }

        case CART_ACTION_TYPES.CLEAR_CART: {
            return cartInitialState
        }

        default:
            return state
    }
}
