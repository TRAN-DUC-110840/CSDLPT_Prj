using CSDLPT_API.Context;
using CSDLPT_API.Enums;
using CSDLPT_API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CSDLPT_API.Utils;

public class serverSwitcherHelper
{
    private readonly K1DBContext _K1context;
    
    private readonly K2DBContext _K2context;
    
    private readonly MyDbContext _myDbContext;

    public serverSwitcherHelper(K1DBContext k1context, K2DBContext k2context, MyDbContext myDbContext)
    {
        this._K1context = k1context;
        this._K2context = k2context;
        this._myDbContext = myDbContext;
    }
    public IDbContext serverChangerHelper(serverEnum serverEnum)
    {
        if (serverEnum == serverEnum.ServerK1)
        {
            return _K1context;
        }
        else if(serverEnum == serverEnum.ServerK2)
        {
            return _K2context;
        }
        else
        {
            return _myDbContext;
        }
    }
}