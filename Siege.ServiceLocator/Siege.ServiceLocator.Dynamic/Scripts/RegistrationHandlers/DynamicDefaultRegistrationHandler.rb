class DefaultRegistrationHandler

    def Handle(component)
        x = Given[component.base].method(:ConstructWith).call(|x| component.type.new())
        x
    end

end